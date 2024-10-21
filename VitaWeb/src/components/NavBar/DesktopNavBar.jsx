import { NavLink, useLocation } from "react-router-dom";
import { motion } from "framer-motion";
import { useAuth } from "../../context/AuthContext";

const menuItems = [
	{ title: "VIDEO", path: "/video" },
	{ title: "KALENDER", path: "/calendar" },
];

export default function DesktopNavBar() {
	const location = useLocation();
	const { user } = useAuth();

	return (
		<div className="flex gap-8">
			{menuItems.map((item, index) => (
				<NavLink
					className="relative hover:text-slate-500"
					key={index}
					to={item.path}
				>
					<p>{item.title}</p>
					{location.pathname === item.path && (
						<motion.div
							layoutId="underline"
							className="absolute -bottom-1 left-0 right-0 h-[1px] bg-black"
						/>
					)}
				</NavLink>
			))}
		</div>
	);
}
