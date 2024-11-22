import { Outlet, Navigate } from "react-router-dom";
import NavBar from "./components/NavBar";
import { useAuth } from "../../hooks/useAuth";
import { useMediaQuery } from "react-responsive";
import MobileNavBar from "./components/MobileNavBar";

const Layout = () => {
	const isMobile = useMediaQuery({ query: "(max-width: 1024px)" });
	const { user } = useAuth();

	if (!user) {
		return <Navigate to="/login" replace />;
	}

	return (
		<div className="h-dvh flex flex-col">
			{isMobile ? <MobileNavBar /> : <NavBar />}
			<div className="flex-1 p-5 px-6 overflow-hidden">
				<Outlet />
			</div>
		</div>
	);
};

export default Layout;
