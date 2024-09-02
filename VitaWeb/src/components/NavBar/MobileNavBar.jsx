import UserCard from "../UserCard";
import { NavLink } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import LoginButton from "../LoginButton";
import LogoutButton from "../LogoutButton";
import NonUserCard from "../NonUserCard";
import { useState } from "react";
import { IoIosMenu, IoIosClose } from "react-icons/io";

export default function MobileNavBar() {
	const { user, isAuthenticated } = useAuth0();
	const [isOpen, setIsOpen] = useState("-translate-y-full");

	return (
		<>
			<button
				className="md:hidden absolute top-3 right-3"
				onClick={() => {
					if (isOpen === "-translate-y-full") {
						setIsOpen("");
					} else {
						setIsOpen("-translate-y-full");
					}
				}}
			>
				<IoIosMenu className="h-8 w-10" />
			</button>
			<div
				className={`h-dvh w-full flex flex-col lg:flex-row bg-gray-300 absolute ${isOpen} z-10 transition-all ease-in-out`}
			>
				<button
					className="md:hidden absolute top-2 right-2"
					onClick={() => {
						if (isOpen === "-translate-y-full") {
							setIsOpen("");
						} else {
							setIsOpen("-translate-y-full");
						}
					}}
				>
					<IoIosClose className="h-11 w-11" />
				</button>
				{isAuthenticated ? (
					<div className="h-full flex flex-col items-center justify-center px-10 py-4 mb-2 lg:mb-0">
						<UserCard user={user} />
						<NavLink
							className="mt-4 bg-gray-200 w-full h-10 rounded-md justify-center flex items-center shadow hover:border hover:border-black"
							to="/"
						>
							<p>Home</p>
						</NavLink>
						<NavLink
							className="mt-4 bg-gray-200 w-full h-10 rounded-md justify-center flex items-center shadow hover:border hover:border-black"
							to="/video"
						>
							<p>Upload Film</p>
						</NavLink>
						<LogoutButton />
					</div>
				) : (
					<div className="flex flex-col items-center justify-center px-10 mb-2 lg:mb-0">
						<NonUserCard />
						<LoginButton />
					</div>
				)}
			</div>
		</>
	);
}
