import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { AuthProvider } from "./contexts/authContext.jsx";
import "./index.css";
import Placeholder from "./Placeholder.jsx";

const router = createBrowserRouter([
	{
		path: "/",
		element: <Placeholder text={"Main Page"} />,
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
		element: <Placeholder text={"Login Page"} />,
	},
]);

createRoot(document.getElementById("root")).render(
	<StrictMode>
		<AuthProvider>
			<RouterProvider router={router} />
		</AuthProvider>
	</StrictMode>
);
