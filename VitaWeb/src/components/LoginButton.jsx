import { useAuth0 } from "@auth0/auth0-react";

export default function LoginButton() {
	const { loginWithRedirect } = useAuth0();

	return (
		<button
			className="w-40 h-10 rounded text-white bg-blue-600 shadow-depth_blue active:shadow hover:bg-blue-700"
			onClick={() => loginWithRedirect()}
		>
			Log Ind
		</button>
	);
}
