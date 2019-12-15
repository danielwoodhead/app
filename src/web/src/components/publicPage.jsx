import React from 'react';
import { Link } from 'react-router-dom';

export const PublicPage = () => {
    return (
        <div>
            Public page
            <br />
            <Link to={`/private`}>Private</Link>
        </div>
    );
};