import logo from "../../../assets/randomLogo.png";
import { NavLink } from "react-router-dom";

const NavBar = () => {
	return (
		<div className="bg-quaternary text-white py-4 flex">
			<div className="mx-10">
				<img src={logo} className="w-10" />
			</div>
			<nav className="flex items-center font-lato">
				<ul className="flex justify-center space-x-6">
					<NavLink className="py-2 px-3 rounded-lg bg-tertiary bg-opacity-40">
						<span>Home</span>
					</NavLink>
					<NavLink className="py-2 px-3 rounded-lg *:opacity-70 *:hover:opacity-100 hover:bg-tertiary hover:bg-opacity-10">
						<span>Kalender</span>
					</NavLink>
					<NavLink className="py-2 px-3 rounded-lg *:opacity-70 *:hover:opacity-100 hover:bg-tertiary hover:bg-opacity-10">
						<span>Videoer</span>
					</NavLink>
					<NavLink className="py-2 px-3 rounded-lg *:opacity-70 *:hover:opacity-100 hover:bg-tertiary hover:bg-opacity-10">
						<span>Opslagstavle</span>
					</NavLink>
				</ul>
			</nav>
		</div>
	);
};

export default NavBar;
