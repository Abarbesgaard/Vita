import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { AuthProvider } from "./contexts/authContext.jsx";
import "./index.css";
import Placeholder from "./Placeholder.jsx";
import Layout from "./pages/layout/Layout.jsx";
import LoginPage from "./pages/auth/LoginPage.jsx";

const router = createBrowserRouter([
	{
		path: "/",
		element: <Layout />,
		children: [
			{
				path: "/video",
				element: <Placeholder text={"Video Page"} />,
			},
			{
				path: "/kalender",
				element: <Placeholder text={"Kalender Page"} />,
			},
			{
				path: "/opslagstavle",
				element: <Placeholder text={"Opslagstavle Page"} />,
			},
		],
	},
	{
		path: "/login",
		element: <LoginPage />,
	},
]);

createRoot(document.getElementById("root")).render(
	<StrictMode>
		<AuthProvider>
			<RouterProvider router={router} />
		</AuthProvider>
	</StrictMode>
);
