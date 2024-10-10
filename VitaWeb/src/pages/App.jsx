import { Route, Routes } from "react-router-dom";
import { useEffect, useState, useContext } from "react";
import Dashboard from "./Dashboard";
import Home from "./Home";
import CalendarPage from "./CalendarPage";
import { ImSpinner2 } from "react-icons/im";
import { IconContext } from "react-icons";
import Login from "./Login";

function App() {
	// if (!currentUser) {
	// 	return (
	// 		<IconContext.Provider value={{ size: "2em" }}>
	// 			<div className="w-full h-dvh text-center flex items-center justify-center">
	// 				<ImSpinner2 className="animate-spin" />
	// 			</div>
	// 		</IconContext.Provider>
	// 	);
	// }

	return (
		<Routes>
			<Route path="/" element={<Home />} />
			<Route path="/login" element={<Login />} />
			<Route path="/video" element={<Dashboard />} />
			<Route path="/calendar" element={<CalendarPage />} />
		</Routes>
	);
}

export default App;
