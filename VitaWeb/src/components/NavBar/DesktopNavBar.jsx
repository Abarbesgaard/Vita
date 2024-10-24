import { NavLink, useLocation } from "react-router-dom";
import { motion } from "framer-motion";
import { useAuth } from "../../context/AuthContext";
import { MdLogout, MdAccountCircle } from "react-icons/md";
import LoginButton from "../Auth/LoginButton";

const menuItems = [
	{ title: "VIDEO", path: "/video" },
	{ title: "KALENDER", path: "/calendar" },
];

export default function DesktopNavBar() {
	const location = useLocation();
	const { user, signOut } = useAuth();

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
			<div className="absolute right-5 ">
				{user ? (
					<div className="flex items-center">
						<MdAccountCircle className="text-3xl" />
						<p className="mx-2">-</p>
						<p className="font-bold mr-10">{user.user_metadata.fullName}</p>
						<div className="ml-2 w-10">
							<MdLogout
								title="Log ud"
								onClick={async () => await signOut()}
								className="hover:text-3xl transition-all text-red-600 text-2xl cursor-pointer"
							/>
						</div>
					</div>
				) : (
					<LoginButton />
				)}
			</div>
		</div>
	);
}
