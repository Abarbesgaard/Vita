import UserCard from "../UserCard";
import { NavLink } from "react-router-dom";
import LoginButton from "../Auth/LoginButton";
import LogoutButton from "../Auth/LogoutButton";
import NonUserCard from "../NonUserCard";
import { IoIosClose } from "react-icons/io";
import { createPortal } from "react-dom";
import { motion } from "framer-motion";
import { useAuth } from "../../context/AuthContext";

export default function MobileNavBar({ onClose }) {
	const { user } = useAuth();

	return (
		<>
			{createPortal(
				<motion.div
					className="h-screen w-screen absolute top-0 backdrop-blur backdrop-brightness-50 items-center justify-center flex"
					initial={{ opacity: 0 }}
					animate={{ opacity: 1 }}
					exit={{ opacity: 0 }}
					onClick={onClose}
				>
					<motion.div
						initial={{ y: "-50%" }}
						animate={{ y: 0 }}
						exit={{ y: "-50%" }}
						className="w-full flex lg:flex-row justify-center items-center bg-gray-300 absolute z-10"
						onClick={(e) => e.stopPropagation()}
					>
						<button
							className="absolute top-2 right-2"
							onClick={() => {
								onClose();
							}}
						>
							<IoIosClose className="h-11 w-11" />
						</button>
						{user ? (
							<div className="h-full w-2/3 flex flex-col md:flex-row items-center justify-center px-10 py-4 mb-2 lg:mb-0">
								<UserCard user={user} />
								<div className="w-full h-full flex flex-col items-center">
									<div className="flex flex-col my-auto space-y-2 w-full sm:w-1/2">
										<NavLink
											className="bg-gray-200 w-full h-10 rounded-md justify-center flex items-center shadow hover:bg-blue-500 hover:text-white transition-all"
											to="/"
										>
											<p>Home</p>
										</NavLink>
										<NavLink
											className="bg-gray-200 w-full h-10 rounded-md justify-center flex items-center shadow hover:bg-blue-500 hover:text-white transition-all"
											to="/video"
										>
											<p>Upload Film</p>
										</NavLink>
										<NavLink
											className="bg-gray-200 w-full h-10 rounded-md justify-center flex items-center shadow hover:bg-blue-500 hover:text-white transition-all"
											to="/calendar"
										>
											<p>Kalender</p>
										</NavLink>
									</div>
									<div className="flex justify-center mt-10">
										<LogoutButton />
									</div>
								</div>
							</div>
						) : (
							<div className="flex flex-col items-center justify-center px-10 mb-2 lg:mb-0">
								<NonUserCard />
								<LoginButton />
							</div>
						)}
					</motion.div>
				</motion.div>,
				document.getElementById("root")
			)}
		</>
	);
}
