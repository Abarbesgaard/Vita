import MobileNavBar from "./NavBar/MobileNavBar";
import { IoIosMenu } from "react-icons/io";
import { useState } from "react";
import { AnimatePresence } from "framer-motion";

export default function Layout({ children }) {
	const [showNavBar, setShowNavBar] = useState(false);

	return (
		<div className="h-dvh">
			<div className="bg-slate-400 w-full h-full flex flex-col md:flex-row items-center overflow-auto">
				<button
					className="absolute top-3 right-3"
					onClick={() => {
						setShowNavBar(true);
					}}
				>
					<IoIosMenu className="h-8 w-10" />
				</button>
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
