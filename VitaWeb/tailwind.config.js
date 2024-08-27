/** @type {import('tailwindcss').Config} */
export default {
	content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
	theme: {
		extend: {
			boxShadow: {
				depth_blue: "inset 0 1px 0 #60a5fa, 0 1px 3px hsla(0, 0%, 0%, 0.3)",
				depth_red: "inset 0 1px 0 #f87171, 0 1px 3px hsla(0, 0%, 0%, 0.3)",
				depth_gray:
					"0 -2px 0 hsla(0, 0%, 100%, 0.15), inset 0 2px 2px hsla(0, 0%, 0%, 0.1)",
			},
		},
	},
	plugins: [],
};
