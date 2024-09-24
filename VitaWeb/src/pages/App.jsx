import { useAuth0 } from "@auth0/auth0-react";
import { Route, Routes } from "react-router-dom";
import Dashboard from "./Dashboard";
import Home from "./Home";
import AuthenticationGuard from "../components/Auth/AuthenticationGuard";
import CalendarPage from "./CalendarPage";
import { ImSpinner2 } from "react-icons/im";
import { IconContext } from "react-icons";

function App() {
	const { isLoading } = useAuth0();

	if (isLoading) {
		return (
			<IconContext.Provider value={{ size: "2em" }}>
				<div className="w-full h-dvh text-center flex items-center justify-center">
					<ImSpinner2 className="animate-spin" />
				</div>
			</IconContext.Provider>
		);
	}

	return (
		<Routes>
			<Route path="/" element={<Home />} />
			<Route path="/video" element={<Dashboard />} />
			<Route path="/calendar" element={<CalendarPage />} />
		</Routes>
	);
}

export default App;
