import { useAuth0 } from "@auth0/auth0-react";
import { Route, Routes } from "react-router-dom";
import Dashboard from "./Dashboard";
import Home from "./Home";
import AuthenticationGuard from "../components/AuthenticationGuard";
import CalendarPage from "./CalendarPage";

function App() {
	const { isLoading } = useAuth0();

	if (isLoading) {
		return (
			<div className="w-full h-dvh text-center flex items-center justify-center">
				<p>Loading...</p>
			</div>
		);
	}

	return (
		<Routes>
			<Route path="/" element={<Home />} />
			<Route
				path="/video"
				element={<AuthenticationGuard component={Dashboard} />}
			/>
			<Route path="/calendar" element={<CalendarPage />} />
		</Routes>
	);
}

export default App;
