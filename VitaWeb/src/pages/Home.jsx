import { motion } from "framer-motion";
import { Link } from "react-router-dom";

export default function Home() {
	return (
		<div className="flex h-dvh items-center justify-center bg-[url('https://www.vitahus.dk/wp-content/uploads/Vitahus-Logo-Web.png')] bg-center bg-no-repeat">
			<div className="h-full w-full flex items-center justify-center bg-black bg-opacity-15">
				<Link to="/video">
					<motion.div
						className="relative bg-opacity-40 w-40 h-52 rounded-lg shadow-lg mx-20"
						whileHover={{
							scale: 1.1,
							rotate: 2,
						}}
						whileTap={{ scale: 0.9 }}
						transition={{ type: "spring", stiffness: 400, damping: 10 }}
					>
						<div className="absolute -right-1 -top-1 w-3 h-3 bg-green-600 rounded-full animate-ping"></div>
						<div className="absolute -right-1 -top-1 w-3 h-3 bg-green-600 rounded-full z-10"></div>
						<img
							className="h-full opacity-50 rounded-lg"
							src="https://thumbs.dreamstime.com/z/upload-video-icon-web-eps-file-easy-to-edit-318039386.jpg?ct=jpeg"
						/>
						<div className="absolute rounded-b-lg py-2 w-full bg-white bottom-0 left-1/2 -translate-x-1/2 text-center font-mono font-bold">
							Video
						</div>
					</motion.div>
				</Link>
				<motion.div
					className="bg-blue-500 bg-opacity-40 w-40 h-52 rounded-lg shadow-lg mx-20 mr-52"
					whileHover={{ scale: 1.1, rotate: -2 }}
					whileTap={{ scale: 0.9 }}
					transition={{ type: "spring", stiffness: 400, damping: 10 }}
				></motion.div>
				<motion.div
					className="bg-blue-500 bg-opacity-40 w-40 h-52 rounded-lg shadow-lg mx-20 ml-52"
					whileHover={{ scale: 1.1, rotate: 2 }}
					whileTap={{ scale: 0.9 }}
					transition={{ type: "spring", stiffness: 400, damping: 10 }}
				></motion.div>
				<motion.div
					className="bg-blue-500 bg-opacity-40 w-40 h-52 rounded-lg shadow-lg mx-20"
					whileHover={{ scale: 1.1, rotate: -2 }}
					whileTap={{ scale: 0.9 }}
					transition={{ type: "spring", stiffness: 400, damping: 10 }}
				></motion.div>
			</div>
		</div>
	);
}
