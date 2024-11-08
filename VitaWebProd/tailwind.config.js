/** @type {import('tailwindcss').Config} */
export default {
	content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
	theme: {
		extend: {
			colors: {
				primary: "#F9F7F7",
				secondary: "#DBE2EF",
				tertiary: "#3F72AF",
				quaternary: "#112D4E",
			},
			fontFamily: {
				lato: ["Lato"],
			},
		},
	},
	plugins: [],
};
