import React, { useState } from 'react'
import supabase from '../Services/supabase'

export default function Home() {

    useEffect(() => {
        const session =supabase.auth.session(
            console.log(session)
        )
    }, [])

    return (
        <button>
			Sign in
		</button>
	);
}
