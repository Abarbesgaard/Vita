import { createRoot } from "react-dom/client";
import { Auth0Provider } from "@auth0/auth0-react";
import { BrowserRouter } from "react-router-dom";
import App from "./pages/App.jsx";
import "./index.css";

const domain = import.meta.env.VITE_AUTH0_DOMAIN;
const clientId = import.meta.env.VITE_AUTH0_CLIENTID;

createRoot(document.getElementById("root")).render(
	<BrowserRouter>
		<Auth0Provider
			domain={domain}
			clientId={clientId}
			authorizationParams={{
				redirect_uri: window.location.origin,
			}}
		>
			<App />
		</Auth0Provider>
	</BrowserRouter>
);
