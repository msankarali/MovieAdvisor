import { RouteObject, Navigate, createBrowserRouter } from "react-router-dom";
import App from "../../App";
import MoviesPage from "../../features/movies/MoviesPage";

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App />,
        children: [
            { path: 'movies', element: <MoviesPage /> },
            { path: '*', element: <Navigate replace to='/not-found' /> },
        ]
    }
]

export const router = createBrowserRouter(routes);