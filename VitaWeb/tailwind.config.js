/** @type {import('tailwindcss').Config} */
export default {
	content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
	theme: {
		extend: {
			boxShadow: {
				depth_blue: "inset 0 1px 0 #60a5fa, 0 1px 3px hsla(0, 0%, 0%, .3)",
				depth_red: "inset 0 1px 0 #f87171, 0 1px 3px hsla(0, 0%, 0%, .3)",
			},
		},
	},
	plugins: [],
};
