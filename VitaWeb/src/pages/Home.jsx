import { motion } from "framer-motion";
import { Link } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { Navigate } from "react-router-dom";
import { IconContext } from "react-icons";
import { RiFolderVideoLine } from "react-icons/ri";

export default function Home() {
	const { user } = useAuth();

	if (!user) {
		return <Navigate to="/login" />;
	}

	// createUser("test@test.dk", "Test1234", "Aske Lysgaard", "asly");

	return (
		<div className="flex h-dvh items-center justify-center bg-[url('https://www.vitahus.dk/wp-content/uploads/Vitahus-Logo-Web.png')] bg-center bg-no-repeat">
			<div className="h-full w-full flex items-center justify-center bg-black bg-opacity-15">
				<Link to="/video">
					<motion.div
						className="bg-white bg-opacity-25 w-40 h-52 rounded-lg shadow-lg mx-20 flex flex-col items-center justify-center"
						whileHover={{
							scale: 1.1,
							rotate: 1,
						}}
						whileTap={{ scale: 0.9 }}
						transition={{ type: "spring", stiffness: 400, damping: 10 }}
					>
						<div className="h-full w-full flex items-center justify-center">
							<IconContext.Provider value={{ size: "8em" }}>
								<RiFolderVideoLine />
							</IconContext.Provider>
						</div>
						<div className="rounded-b-lg mt-auto h-1/6 w-full bg-white font-mono font-bold flex justify-center items-center">
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
