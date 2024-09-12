import DesktopNavBar from "./NavBar/DesktopNavBar";
import MobileNavBar from "./NavBar/MobileNavBar";

export default function Layout({ children }) {
	return (
		<div className="h-dvh">
			<div className="bg-slate-400 w-full h-full flex flex-col md:flex-row items-center overflow-auto">
				<MobileNavBar />
				<DesktopNavBar />
				{children}
			</div>
		</div>
	);
}
