import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { AuthProvider } from "./contexts/authContext.jsx";
import { VideoProvider } from "./contexts/videoContext.jsx";
import "./index.css";
import Layout from "./pages/layout/Layout.jsx";
import LoginPage from "./pages/auth/LoginPage.jsx";
import NoticeBoard from "./pages/noticeboard/NoticeBoardPage.jsx";
import VideoPage from "./pages/video/VideoPage.jsx";
import CalendarPage from "./pages/calendar/CalendarPage.jsx";
import HomePage from "./pages/home/HomePage.jsx";

const router = createBrowserRouter([
	{
		path: "/",
		element: <Layout />,
		children: [
			{
				index: true,
				element: <HomePage />,
			},
			{
				path: "/video",
				element: <VideoPage />,
			},
			{
				path: "/kalender",
				element: <CalendarPage />,
			},
			{
				path: "/opslagstavle",
				element: <NoticeBoard />,
			},
		],
	},
	{
		path: "/login",
		element: <LoginPage />,
	},
]);

createRoot(document.getElementById("root")).render(
	<AuthProvider>
		<VideoProvider>
			<RouterProvider router={router} />
		</VideoProvider>
	</AuthProvider>
);
