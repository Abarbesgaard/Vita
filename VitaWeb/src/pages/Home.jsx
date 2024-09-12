import { motion } from "framer-motion";
import { Link } from "react-router-dom";

export default function Home() {
	return (
		<div className="flex h-dvh items-center justify-center bg-[url('https://www.vitahus.dk/wp-content/uploads/Vitahus-Logo-Web.png')] bg-center bg-no-repeat">
			<div className="h-2/5 w-full flex items-center justify-center">
				<Link to="/video">
					<motion.div
						className="bg-blue-500 bg-opacity-40 w-40 h-52 rounded-lg shadow-lg mx-20"
						whileHover={{ scale: 1.1, rotate: 2 }}
						whileTap={{ scale: 0.9 }}
						transition={{ type: "spring", stiffness: 400, damping: 10 }}
					></motion.div>
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
