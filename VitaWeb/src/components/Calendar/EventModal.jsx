import { createPortal } from "react-dom";
import { motion } from "framer-motion";

const EventModal = ({ onClose, event, resources }) => {
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
						<div className="flex justify-between items-center p-5">
							<h2 className="text-xl font-bold">{event.title}</h2>
							<button onClick={onClose}>X</button>
						</div>
						<div className="p-5">
							<p className="my-10">{event.description}</p>
							<p className="font-bold text-lg">Deltagere:</p>
							{event.resourceId.map((id) => (
								<p key={id}>{resources.find((r) => r.id === id).title}</p>
							))}
						</div>
					</motion.div>
				</motion.div>,
				document.getElementById("root")
			)}
		</>
	);
};

export default EventModal;
