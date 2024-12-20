import { createPortal } from "react-dom";
import { motion } from "framer-motion";
import { useState } from "react";
import { createActivity } from "../../../APIs/calendarAPI";
import { getSessionToken } from "../../../services/supabase";
import { PiSpinnerGap } from "react-icons/pi";

const AddEventModal = ({ onClose, users, user, setEvents, selectedSlot }) => {
	const [isLoading, setIsLoading] = useState(false);
	const [event, setEvent] = useState({
		attendee: [{ id: user.id, name: user.name }],
		allDayEvent: false,
		hostId: user.id,
		title: "",
		start: selectedSlot
			? new Date(selectedSlot.start.setHours(selectedSlot.start.getHours() + 1))
			: new Date(),
		end: selectedSlot
			? new Date(selectedSlot.end.setHours(selectedSlot.end.getHours() + 1))
			: new Date(),
	});

	const saveActivitytoDb = async () => {
		setIsLoading(true);
		const token = await getSessionToken();
		const response = await createActivity(
			{
				...event,
				resourceId: event.attendee.map((attendee) => attendee.id),
			},
			token
		);
		setEvents((prev) => [
			...prev,
			{
				...event,
				id: response.id,
				allDay: event.allDayEvent,
				resourceId: event.attendee.map((attendee) => attendee.id),
				accepted: [],
			},
		]);
		onClose();
	};

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
					>
						<div
							className={`flex justify-between p-5 bg-blue-500 rounded-t-lg`}
						>
							<p className="text-white text-xl font-bold">Ny Begivenhed</p>
							<button onClick={onClose} className="ml-auto">
								X
							</button>
						</div>
						<div className="p-5">
							<textarea
								placeholder="Navn på begivenhed"
								value={event.title}
								onChange={(e) => setEvent({ ...event, title: e.target.value })}
								className="py-1 px-2 w-full bg-gray-100 shadow-inner resize-none"
							/>
							<textarea
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
										value={event.start.toISOString().slice(0, 16)}
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
										value={event.end.toISOString().slice(0, 16)}
										onChange={(e) =>
											setEvent({ ...event, end: new Date(e.target.value) })
										}
									/>
								</div>
								<div>
									{users.map((user) => (
										<div className="space-x-2 flex" key={user.id}>
											<input
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
