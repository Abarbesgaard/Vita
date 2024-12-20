import { Route, Routes } from "react-router-dom";
import VideoPage from "./VideoPage";
import Home from "./Home";
import CalendarPage from "./CalendarPage";
import Login from "./Login";
import NoticeBoard from "./NoticeBoard";

function App() {
	return (
		<Routes>
			<Route path="/" element={<Home />} />
			<Route path="/login" element={<Login />} />
			<Route path="/video" element={<VideoPage />} />
			<Route path="/calendar" element={<CalendarPage />} />
			<Route path="/opslagstavle" element={<NoticeBoard />} />
		</Routes>
	);
}

export default App;
