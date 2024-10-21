import MobileNavBar from "./NavBar/MobileNavBar";
import { IoIosMenu } from "react-icons/io";
import { useState } from "react";
import { AnimatePresence } from "framer-motion";
import DesktopNavBar from "./NavBar/DesktopNavBar";
import { useMediaQuery } from "react-responsive";
import { NavLink } from "react-router-dom";

export default function Layout({ children }) {
	const [showNavBar, setShowNavBar] = useState(false);
	const isMobile = useMediaQuery({ query: "(max-width: 768px)" });
	return (
		<div className="h-screen overflow-hidden">
			<div className="flex p-5 bg-gray-200">
				{!isMobile && (
					<div className="mx-auto">
						<DesktopNavBar />
					</div>
				)}
			</div>
			<div className="w-full h-full flex flex-col md:flex-row items-center">
				{isMobile && (
					<button
						className="absolute top-0 right-0 p-5"
						onClick={() => {
							setShowNavBar(true);
						}}
					>
						<IoIosMenu className="text-5xl" />
					</button>
				)}

				<AnimatePresence>
					{showNavBar && (
						<MobileNavBar
							onClose={() => {
								setShowNavBar(false);
							}}
						/>
					)}
				</AnimatePresence>
				{children}
			</div>
		</div>
	);
}
