import DesktopNavBar from "./NavBar/DesktopNavBar";
import MobileNavBar from "./NavBar/MobileNavBar";

export default function Layout({ children }) {
	return (
		<div className="h-dvh">
			<div className="bg-slate-400 w-full h-full flex flex-col md:flex-row items-center overflow-auto">
				<MobileNavBar />
				<DesktopNavBar />
				{/* <button
					className="md:hidden"
					onClick={() => {
						if (isOpen === "collapse") {
							setIsOpen("");
						} else {
							setIsOpen("collapse");
						}
					}}
				>
					Menu
				</button>
				<div
					className={`h-dvh w-full flex flex-col lg:flex-row bg-gray-300 absolute ${isOpen} z-10`}
				>
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
				</div> */}

				{children}
			</div>
		</div>
	);
}
