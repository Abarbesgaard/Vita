import { createPortal } from "react-dom";
import { motion } from "framer-motion";
import ReactPlayer from "react-player/youtube";
import { IoClose } from "react-icons/io5";

const VideoModal = ({ onClose, url }) => {
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
						className="bg-black w-1/2 rounded-lg shadow-2xl *:*:hover:block"
						onClick={(e) => e.stopPropagation()}
						initial={{ scale: 0.5 }}
						animate={{ scale: 1 }}
						transition={{ type: "spring", stiffness: 260, damping: 20 }}
						exit={{ scale: 0.5 }}
					>
						<div className="flex justify-end p-5 h-20 rounded-t-lg">
							<IoClose
								className="text-3xl text-white cursor-pointer hidden"
								onClick={onClose}
							/>
						</div>
						<div className="flex items-center justify-center w-full p-5 pt-0 pb-20">
							<ReactPlayer
								url={url}
								controls={true}
								playing={true}
								width="100%"
							/>
						</div>
					</motion.div>
				</motion.div>,
				document.getElementById("root")
			)}
		</>
	);
};

export default VideoModal;
