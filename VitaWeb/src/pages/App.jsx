import { useAuth0 } from "@auth0/auth0-react";
import LoginButton from "../components/LoginButton";
import Dashboard from "./Dashboard";

function App() {
	const { user, isAuthenticated, isLoading, error } = useAuth0();

	if (error) {
		return <p>{error}</p>;
	}

	if (isLoading) {
		return (
			<div className="mx-auto h-dvh text-center">
				<p>Loading...</p>
			</div>
		);
	}

	return isAuthenticated ? (
		<Dashboard user={user} />
	) : (
		<div className="h-dvh flex justify-center items-center">
			<LoginButton />
		</div>
	);
}

export default App;
