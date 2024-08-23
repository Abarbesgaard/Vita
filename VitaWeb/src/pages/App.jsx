import { useAuth0 } from "@auth0/auth0-react";
import LoginButton from "../components/LoginButton";
import LogoutButton from "../components/LogoutButton";

function App() {
	const { user, isAuthenticated, isLoading, error } = useAuth0();

	if (error) {
		return <p>{error}</p>;
	}

	if (isLoading) {
		return <div className="mx-auto text-center">Loading...</div>;
	}

	return isAuthenticated ? (
		<div className="h-dvh flex flex-col items-center justify-center space-y-4">
			<p>Logget ind som:</p>
			<p className="text-2xl">{user.name}</p>
			<p className="text-1xl">{user.email}</p>
			<LogoutButton />
		</div>
	) : (
		<div className="h-dvh flex justify-center items-center">
			<LoginButton />
		</div>
	);
}

export default App;
