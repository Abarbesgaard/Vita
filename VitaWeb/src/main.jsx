import { createRoot } from "react-dom/client";
import {
	BrowserRouter,
	createBrowserRouter,
	RouterProvider,
} from "react-router-dom";
import App from "./pages/App.jsx";
import "./index.css";
import AuthProvider from "./context/AuthContext.jsx";
import Layout from "./components/Layout.jsx";
import Home from "./pages/Home.jsx";
import VideoPage from "./pages/VideoPage.jsx";
import CalendarPage from "./pages/CalendarPage.jsx";
import Login from "./pages/Login.jsx";

const router = createBrowserRouter([
	{
		path: "/",
		element: <Layout />,
		children: [
			{
				index: true,
				element: <Home />,
			},
			{
				path: "/video",
				element: <VideoPage />,
			},
			{
				path: "/calendar",
				element: <CalendarPage />,
			},
		],
	},
	{
		path: "/login",
		element: <Login />,
	},
]);

createRoot(document.getElementById("root")).render(
	<AuthProvider>
		{/* <BrowserRouter>
			<App />
		</BrowserRouter> */}
		<RouterProvider router={router} />
	</AuthProvider>
);
