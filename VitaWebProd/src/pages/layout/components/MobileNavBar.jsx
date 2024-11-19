import { IoHomeSharp, IoCalendar } from "react-icons/io5";
import { NavLink, useLocation } from "react-router-dom";
import { MdSlowMotionVideo } from "react-icons/md";
import { LuMessagesSquare } from "react-icons/lu";
import { useState } from "react";
import { FaUserCircle } from "react-icons/fa";
import { motion } from "framer-motion";
import { useAuth } from "../../../hooks/useAuth";

const links = [
	{ text: "Hjem", to: "/", icon: <IoHomeSharp className="text-xl" /> },
	{
		text: "Kalender",
		to: "/kalender",
		icon: <IoCalendar className="text-xl" />,
	},
	{
		text: "Videoer",
		to: "/video",
		icon: <MdSlowMotionVideo className="text-2xl" />,
	},
	{
		text: "Opslagstavle",
		to: "/opslagstavle",
		icon: <LuMessagesSquare className="text-xl" />,
	},
];

const MobileNavBar = () => {
	const location = useLocation();
	const [activeLink, setActiveLink] = useState(
		links.find((link) => link.to === location.pathname)
	);
	const { signOut } = useAuth();

	return (
		<div className="bg-quaternary text-white flex py-4">
			<nav className="h-full ml-6 flex items-center font-lato">
				<ul className="flex justify-center space-x-3">
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
									className="absolute w-full h-0.5 bottom-0 left-0 bg-secondary rounded-sm"
								></motion.div>
							) : null}
							{link.icon}
						</NavLink>
					))}
				</ul>
			</nav>
			<div className="ml-auto mr-6 flex items-center">
				<FaUserCircle className="text-2xl cursor-pointer" onClick={signOut} />
			</div>
		</div>
	);
};

export default MobileNavBar;
