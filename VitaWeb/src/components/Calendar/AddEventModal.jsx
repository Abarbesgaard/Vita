import { createPortal } from "react-dom";
import { motion } from "framer-motion";
import { useState } from "react";
import { saveActivity } from "../../API/ActivityAPI";
import { getSessionToken } from "../../services/Supabase";
import { PiSpinnerGap } from "react-icons/pi";
import moment from "moment";
import { IoClose } from "react-icons/io5";

moment.locale("da-DK", {
	culture: "da-DK",
});

const AddEventModal = ({ onClose, users, user, setEvents }) => {
	const [isLoading, setIsLoading] = useState(false);
	const [event, setEvent] = useState({
		attendee: [],
		allDayEvent: false,
		hostId: user.id,
		title: "",
		start: moment().toDate(),
		end: new Date(new Date().getTime() + 60 * 60 * 1000),
		description: "",
	});

	const saveActivitytoDb = async () => {
		setIsLoading(true);
		const token = await getSessionToken();
		const response = await saveActivity(event, token);
		setEvents((prev) => [
			...prev,
			{
				...response,
				resourceId: response.attendee.map((attendee) => attendee.id),
				allDay: response.allDayEvent,
				accepted: response.verifiedAttendee,
				start: new Date(response.start),
				end: new Date(response.end),
			},
		]);
		onClose();
	};

	console.log(event.start.toLocaleString({ format: "YYYY-MM_DDHH:mm" }));
	console.log(event.start.toISOString());
	console.log(moment(event.start).format("YYYY-MM-DDTHH:mm"));

	return (
		<>
			{createPortal(
				<motion.div
					className="w-screen h-screen top-0 absolute bg-black bg-opacity-50 backdrop-blur-sm z-10 flex items-center justify-center"
					onClick={onClose}
					initial={{ opacity: 0 }}
					animate={{ opacity: 1 }}
					exit={{ opacity: 0 }}
				>
					<motion.div
						className="bg-white w-1/2 rounded-lg shadow-2xl"
						onClick={(e) => e.stopPropagation()}
						initial={{ scale: 0.5 }}
						animate={{ scale: 1 }}
						transition={{ type: "spring", stiffness: 260, damping: 20 }}
						exit={{ scale: 0.5 }}
					>
						<div
							className={`flex justify-between p-5 bg-blue-500 rounded-t-lg`}
						>
							<h2 className="text-xl text-white font-bold ml-2">
								Ny begivenhed
							</h2>
							<button onClick={onClose}>
								<IoClose className="text-3xl text-white" />
							</button>
						</div>
						<div className="p-5">
							<input
								className="bg-gray-100 shadow-inner p-1 w-full mb-2"
								placeholder="Indsæt titel"
								value={event.title}
								onChange={(e) => setEvent({ ...event, title: e.target.value })}
							/>
							<textarea
								placeholder="Indsæt beskrivelse"
								rows={4}
								value={event.description}
								onChange={(e) =>
									setEvent({ ...event, description: e.target.value })
								}
								className="py-1 px-2 w-full bg-gray-100 shadow-inner resize-none"
							/>
							<div className="space-y-2">
								<div className="flex items-center space-x-2">
									<label className="font-bold">Heldags?</label>
									<input
										type="checkbox"
										value={event.allDayEvent}
										onChange={(e) =>
											setEvent({ ...event, allDayEvent: e.target.checked })
										}
									/>
								</div>
								<div className="flex items-center space-x-2">
									<label className="font-bold">Start dato:</label>
									<input
										className="bg-gray-100 shadow-inner p-1"
										type="datetime-local"
										value={moment(event.start).format("YYYY-MM-DDTHH:mm")}
										onChange={(e) =>
											setEvent({ ...event, start: new Date(e.target.value) })
										}
									/>
								</div>
								<div className="flex items-center space-x-2">
									<label className="font-bold">Slut dato:</label>
									<input
										className="bg-gray-100 shadow-inner p-1"
										type="datetime-local"
										value={moment(event.end).format("YYYY-MM-DDTHH:mm")}
										onChange={(e) =>
											setEvent({ ...event, end: new Date(e.target.value) })
										}
									/>
								</div>
								<div>
									{users.map((user) => (
										<div className="space-x-2 flex" key={user.id}>
											<input
												className="scale-125"
												type="checkbox"
												checked={event.attendee.some((u) => u.id === user.id)}
												onChange={() => {
													if (event.attendee.some((u) => u.id === user.id)) {
														setEvent({
															...event,
															attendee: event.attendee.filter(
																(u) => u.id !== user.id
															),
														});
													} else {
														setEvent({
															...event,
															attendee: [...event.attendee, user],
														});
													}
												}}
											/>
											<label>{user.name}</label>
										</div>
									))}
								</div>
								<div className="w-full flex justify-end">
									<button
										onClick={saveActivitytoDb}
										className="h-10 w-40 rounded bg-blue-500 shadow-md text-white hover:bg-blue-600 active:scale-95 flex justify-center items-center"
									>
										{isLoading ? (
											<PiSpinnerGap className="animate-spin text-2xl" />
										) : (
											"Tilføj begivenhed"
										)}
									</button>
								</div>
							</div>
						</div>
					</motion.div>
				</motion.div>,
				document.getElementById("root")
			)}
		</>
	);
};

export default AddEventModal;
