import { createPortal } from "react-dom";
import { motion } from "framer-motion";
import VideoForm from "./VideoForm";

const AddVideoModal = ({
	setShowAddVideoModal,
	handleVideoFormSubmit,
	title,
	setTitle,
	url,
	setUrl,
	description,
	setDescription,
	mode,
}) => {
	return createPortal(
		<motion.div
			className="w-screen h-screen top-0 absolute bg-black bg-opacity-50 backdrop-blur-sm z-10 flex items-center justify-center"
			initial={{ opacity: 0 }}
			animate={{ opacity: 1 }}
			exit={{ opacity: 0 }}
			onClick={() => {
				setShowAddVideoModal(false);
			}}
		>
			<div
				onClick={(e) => e.stopPropagation()}
				className="h-1/2 w-1/2 bg-white rounded-xl flex justify-center items-center shadow-lg p-10"
			>
				<VideoForm
					handleVideoFormSubmit={handleVideoFormSubmit}
					title={title}
					setTitle={setTitle}
					url={url}
					setUrl={setUrl}
					description={description}
					setDescription={setDescription}
					mode={mode}
				/>
			</div>
		</motion.div>,
		document.getElementById("root")
	);
};

export default AddVideoModal;
