import { createRoot } from "react-dom/client";
import { Auth0Provider } from "@auth0/auth0-react";
import App from "./pages/App.jsx";
import "./index.css";

const domain = import.meta.env.VITE_AUTH0_DOMAIN;
const clientId = import.meta.env.VITE_AUTH0_CLIENTID;

createRoot(document.getElementById("root")).render(
	<Auth0Provider
		domain="dev-dj6iiunlxv3pukjx.us.auth0.com"
		clientId="pNpgTlk2dc6lwuQG00ywgSAh5QBAoXLy"
		authorizationParams={{
			redirect_uri: window.location.origin,
		}}
	>
		<App />
	</Auth0Provider>
);
