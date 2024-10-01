import React from 'react';
import { useNavigate } from 'react-router-dom';
import '../App.css';

export default function NavBar() {
    const navigate = useNavigate();

    return (
        <nav className="navbar">
            <img className='nav-image' src='https://www.vitahus.dk/wp-content/uploads/Vitahus-Logo-Web.png'/>
                <div className="nav-buttons">
                    <button className="nav-button" onClick={() => navigate('/')}>Home</button>
                    <button className="nav-button" onClick={() => navigate('/videos')}>Videos</button>
                    <button className="nav-button" onClick={() => navigate('/calendar')}>Calendar</button>
                </div> 
            </nav>
    );
}
