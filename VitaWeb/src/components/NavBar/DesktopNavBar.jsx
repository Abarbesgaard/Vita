import UserCard from "../UserCard";
import { NavLink } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import LoginButton from "../LoginButton";
import LogoutButton from "../LogoutButton";
import NonUserCard from "../NonUserCard";

export default function DesktopNavBar() {
	const { user, isAuthenticated } = useAuth0();

	return (
		<>
			<div className={`h-dvh w-full md:w-1/5 md:flex bg-gray-300 hidden`}>
				{isAuthenticated ? (
					<div className="flex flex-col items-center justify-center px-10 py-4 mb-2 lg:mb-0">
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
