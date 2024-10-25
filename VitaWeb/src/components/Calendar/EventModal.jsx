import { createPortal } from "react-dom";
import { motion } from "framer-motion";
import { useEffect, useState } from "react";
import { BsCalendarCheckFill } from "react-icons/bs";
import { TbCalendarQuestion } from "react-icons/tb";
import { IoClose } from "react-icons/io5";

const EventModal = ({ onClose, event, resources }) => {
	const [color, setColor] = useState("bg-[#265985]");

	console.log(event);

	useEffect(() => {
		if (event.type === "meeting") {
			setColor("bg-red-500");
		}

		// if (event.cancelled) {
		// 	setColor("bg-gray-500");
		// }
	}, [event.type, event.cancelled]);

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
						className={`bg-white w-1/2 h-5/6 rounded-lg shadow-2xl overflow-y-auto ${
							event.cancelled && "grayscale"
						}`}
						onClick={(e) => e.stopPropagation()}
						initial={{ scale: 0.75 }}
						animate={{ scale: 1 }}
						exit={{ scale: 0.75 }}
					>
						<div className={`flex justify-between p-5 ${color} rounded-t-lg`}>
							<h2
								className={`text-xl text-white font-bold ml-2 ${
									event.cancelled && "line-through"
								}`}
							>
								{event.title}
							</h2>
							<button onClick={onClose}>
								<IoClose className="text-3xl text-white" />
							</button>
						</div>
						<div className={`p-5 ${event.cancelled && "opacity-30"}`}>
							<p>{event.start.toString()}</p>
							<p></p>
							<textarea
								rows={4}
								disabled
								value={event.description}
								className={`py-1 px-2 w-full bg-gray-100 shadow-inner resize-none`}
							/>
							<p className="font-bold text-lg ml-1">Inviterede:</p>
							<div className="flex flex-col w-full px-4 py-2 border-t">
								<div className="flex justify-center">
									<div className="grid grid-rows-auto lg:grid-cols-3 grid-cols-2 grid-flow-row gap-1 w-full">
										{event.attendee.map((attendee) => (
											<div
												key={attendee.id}
												className="flex items-center shadow-inner bg-gray-100 rounded px-2 py-1"
											>
												{event.accepted.some((a) => a.id === attendee.id) ? (
													<div className="flex items-center">
														<BsCalendarCheckFill className="text-green-600 mr-4 text-xl" />
														<div>
															<p>{attendee.name}</p>
															<p className="text-sm opacity-85 font-bold">
																Deltager
															</p>
														</div>
													</div>
												) : (
													<div className="flex items-center">
														<TbCalendarQuestion className="mr-4 scale-125 text-xl" />
														<div>
															<p>{attendee.name}</p>
															<p className="text-sm opacity-40 font-bold">
																Afventer svar
															</p>
														</div>
													</div>
												)}
											</div>
										))}
									</div>
								</div>

								{/* <div>
									<p className="font-bold text-lg">Accepteret:</p>
									{event.accepted &&
										event.accepted.map((attendee) => (
											<p key={attendee.id}>{attendee.name}</p>
										))}
								</div> */}
							</div>
						</div>
					</motion.div>
				</motion.div>,
				document.getElementById("root")
			)}
		</>
	);
};

export default EventModal;
