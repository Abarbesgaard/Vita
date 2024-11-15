import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { AuthProvider } from "./contexts/authContext.jsx";
import "./index.css";
import Placeholder from "./Placeholder.jsx";
import Layout from "./pages/layout/Layout.jsx";
import LoginPage from "./pages/auth/LoginPage.jsx";
import NoticeBoard from "./pages/noticeboard/NoticeBoardPage.jsx";
import VideoPage from "./pages/video/VideoPage.jsx";
import CalendarPage from "./pages/calendar/CalendarPage.jsx";

const router = createBrowserRouter([
	{
		path: "/",
		element: <Layout />,
		children: [
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
			<RouterProvider router={router} />
		</AuthProvider>
);
