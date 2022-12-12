import { BrowserRouter, Route, Routes } from 'react-router-dom';
import './App.css';
import Authenticate from './component/Authenticate';
import Layout from './component/Layout';
import Login from './component/Login';
import Quiz from './component/Quiz';
import Result from './component/Result';

function App() {
  return (
    <BrowserRouter>
    <Routes>
      <Route path="/" element={<Login />} />
      <Route element={<Authenticate />} />
      <Route path="/" element={<Layout />} />
      <Route path="/quiz" element={<Quiz />} />
      <Route path="/result" element={<Result />} />

    </Routes>
    </BrowserRouter>
    );
}

export default App;
