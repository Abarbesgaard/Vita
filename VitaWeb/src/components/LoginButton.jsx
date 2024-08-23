import { useAuth0 } from "@auth0/auth0-react";

export default function LoginButton() {
	const { loginWithRedirect } = useAuth0();

	return (
		<button
			className="w-40 h-10 rounded bg-green-500"
			onClick={() => loginWithRedirect()}
		>
			Log Ind
		</button>
	);
}
