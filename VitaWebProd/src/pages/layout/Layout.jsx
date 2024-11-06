import { Outlet } from "react-router-dom";
import NavBar from "./components/NavBar";

const Layout = () => {
	return (
		<div className="h-dvh flex flex-col bg-primary">
			<NavBar />
			<div className="flex-1 p-5">
				<Outlet />
			</div>
		</div>
	);
};

export default Layout;
