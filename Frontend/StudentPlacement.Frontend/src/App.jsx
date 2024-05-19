import viteLogo from '/vite.svg' // будет как пример
import './App.css'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { AuthProvider } from './components/Context/AuthProvider'
import Auth from './components/Auth/Auth'
import Home from './components/Home/Home'
import ProtectedRoute from './components/Context/ProtectedRoute'
import Account from './components/Account/Account'
import AccountControll from './components/Account/AccountControll'
import Profile from './components/Profile/Profile'

function App() {
  

  return (
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          <Route exact path="/Auth" element={<Auth/>}/>
          <Route exact path="/Account" element={<Account/>}/>
          <Route exact path="/AccountControll" element={<AccountControll/>}/>
          <Route exact path="/Profile" element={<Profile/>}/>
          <Route element={<ProtectedRoute/>}>
            <Route exact path="/Home" element={<Home/>}/>
          </Route>
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  )
}

export default App
