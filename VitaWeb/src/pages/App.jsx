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
		<div className="h-dvh flex flex-col items-center justify-center bg-gray-300">
			<div className="bg-white rounded-xl shadow-md mb-20 flex flex-col md:w-1/5">
				<div className="bg-blue-300 w-full h-28 rounded-t-xl"></div>
				<div className="-mt-28 p-2 pr-20">
					<img
						src={user.picture}
						width={150}
						className="rounded-full border-white border-8 ml-2 mt-2"
					/>
				</div>
				<div className="flex flex-col items-end p-10 pt-1">
					<p className="text-2xl md:text-3xl">{user.name}</p>
					<p className="text-xl md:text-2xl">{user.email}</p>
				</div>
			</div>
			<LogoutButton />
		</div>
	) : (
		<div className="h-dvh flex justify-center items-center">
			<LoginButton />
		</div>
	);
}

export default App;
