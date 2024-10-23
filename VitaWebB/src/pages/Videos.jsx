import React, { useEffect, useState } from 'react';
import { createVideo, deleteVideo, getAllVideos } from '../APIs/VideoAPI';
import VideoCard from '../Components/VideoCard.jsx';

export default function Videos() {
    const [videos, setVideos] = useState([]); 
    
    const [newVideo, setNewVideo] = useState({
        title: '',
        description: '',
        url: ''
    });

    // Fetch all videos when the component mounts
    useEffect(() => {
        fetchVideos();
    }, []);

    const fetchVideos = async () => {
        const videoList = await getAllVideos();
        setVideos(videoList);
        console.log("Fetched videos");
    };

    // Handle input changes for the new video form
    const handleChange = (e) => {
        const { name, value } = e.target;
        setNewVideo((prev) => ({
            ...prev,
            [name]: value
        }));
    };

    // Handle form submission to add a new video
    const handleSubmit = async (e) => {
        e.preventDefault();
        if (newVideo.title && newVideo.description && newVideo.url) {
            const addedVideo = await createVideo(newVideo); 
            if (addedVideo) { 
                setVideos((prev) => [...prev, addedVideo]); 
                setNewVideo({ title: '', description: '', url: '' }); 
            } else {
                console.error("Failed to add video."); 
            }
        }
    };

    // Handle delete video
    const handleDelete = async (id) => {
        try {
            const result = await deleteVideo(id);
            if (result) {
                setVideos((prev) => prev.filter(video => video.id !== id));
            } else {
                console.error('Kunne ikke slette videoen, prøv igen');
            }
        } catch (error) {
            console.error('Fejl ved sletning af video:', error);
        } finally {
            await fetchVideos();
        }
    };

    return (
        <div>
            <h1>Videos</h1>
            <form onSubmit={handleSubmit}>
                <input 
                    key= "title"
                    type="text" 
                    name="title" 
                    placeholder="Title" 
                    value={newVideo.title} 
                    onChange={handleChange} 
                    required 
                />
                <input 
                    key="description"
                    type="text" 
                    name="description" 
                    placeholder="Description" 
                    value={newVideo.description} 
                    onChange={handleChange} 
                    required 
                />
                <input 
                    key="url"
                    type="url" 
                    name="url" 
                    placeholder="Video URL" 
                    value={newVideo.url} 
                    onChange={handleChange} 
                    required 
                />
                <button type="submit">Add Video</button>
            </form>

            <div className="video-list">
                {videos.length === 0 ? (
                    <h2>Tilføj video</h2>
                ) : (
                    videos.map(video => (
                        <VideoCard key={video.id} video={video} onDelete={handleDelete} />
                    ))
                )}
            </div>
        </div>
    );
}