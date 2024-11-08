import logo from "../../../assets/randomLogo.png";
import { FaUserCircle } from "react-icons/fa";
import { IoHomeSharp, IoCalendar } from "react-icons/io5";
import { MdSlowMotionVideo } from "react-icons/md";
import { LuMessagesSquare } from "react-icons/lu";
import { NavLink, useLocation } from "react-router-dom";
import { useState } from "react";
import { motion } from "framer-motion";
import { useAuth } from "../../../contexts/useAuth";

const links = [
	{ text: "Hjem", to: "/", icon: <IoHomeSharp /> },
	{ text: "Kalender", to: "/kalender", icon: <IoCalendar /> },
	{
		text: "Videoer",
		to: "/video",
		icon: <MdSlowMotionVideo className="text-xl" />,
	},
	{
		text: "Opslagstavle",
		to: "/opslagstavle",
		icon: <LuMessagesSquare className="text-lg" />,
	},
];

const NavBar = () => {
	const location = useLocation();
	const [activeLink, setActiveLink] = useState(
		links.find((link) => link.to === location.pathname)
	);
	const { user, signOut } = useAuth();
	console.log(user);
	return (
		<div className="bg-gradient-to-r from-quaternary from-50% to-tertiary text-white py-4 flex sticky top-0 z-10">
			<div className="mx-12">
				<img src={logo} className="w-10" />
			</div>
			<nav className="flex items-center font-lato">
				<ul className="flex justify-center space-x-6">
					{links.map((link, index) => (
						<NavLink
							key={index}
							to={link.to}
							className={({ isActive }) =>
								isActive ? "active-link relative" : "pending-link relative"
							}
							onClick={() => setActiveLink(link)}
						>
							{link === activeLink ? (
								<motion.div
									key={link.text}
									layoutId="active"
									className="absolute w-full h-1 bottom-0 left-0 bg-secondary rounded-sm"
								></motion.div>
							) : null}
							{link.icon}
							<span>{link.text}</span>
						</NavLink>
					))}
				</ul>
			</nav>
			<div className="ml-auto mr-12 flex items-center">
				<p className="mr-2">{user.user_metadata.fullName}</p>
				<FaUserCircle className="text-4xl cursor-pointer" onClick={signOut} />
			</div>
		</div>
	);
};

export default NavBar;
