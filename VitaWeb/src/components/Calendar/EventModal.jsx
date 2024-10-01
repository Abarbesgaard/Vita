import { createPortal } from "react-dom";
import { motion } from "framer-motion";
import { useEffect, useState } from "react";

const EventModal = ({ onClose, event, resources }) => {
	const [color, setColor] = useState("bg-[#265985]");

	useEffect(() => {
		if (event.type === "meeting") {
			setColor("bg-red-500");
		}
	}, [event.type]);

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
						className="bg-white w-1/2 h-2/3 rounded-lg shadow-2xl"
						onClick={(e) => e.stopPropagation()}
						initial={{ scale: 0 }}
						animate={{ scale: 1 }}
						transition={{ type: "spring", stiffness: 260, damping: 20 }}
						exit={{ scale: 0 }}
					>
						<div className={`flex justify-between p-5 ${color} rounded-t-lg`}>
							<h2 className="text-xl text-white font-bold ml-2">
								{event.title}
							</h2>
							<button onClick={onClose}>X</button>
						</div>
						<div className="p-5">
							<textarea
								rows={4}
								disabled
								value={event.description}
								className="py-1 px-2 w-full bg-gray-100 shadow-inner resize-none"
							/>
							<div className="flex justify-evenly w-1/2">
								<div>
									<p className="font-bold text-lg">Inviterede:</p>
									{event.invites
										.filter((id) => {
											return (
												!event.accepted.includes(id) &&
												!event.declined.includes(id)
											);
										})
										.map((id) => (
											<p key={id}>{resources.find((r) => r.id === id).title}</p>
										))}
								</div>
								<div>
									<p className="font-bold text-lg">Deltager:</p>
									{event.accepted.map((id) => (
										<p key={id}>{resources.find((r) => r.id === id).title}</p>
									))}
								</div>
								<div>
									<p className="font-bold text-lg">Deltager ikke:</p>
									{event.declined.map((id) => (
										<p key={id}>{resources.find((r) => r.id === id).title}</p>
									))}
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

export default EventModal;
