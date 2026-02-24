import { useState } from "react";

function App() {
    const [hovering, setHovering] = useState(false);
    const [loading, setLoading] = useState(false);

    const handleGoogleLogin = () => {
        setLoading(true);
        const returnUrl = encodeURIComponent("https://localhost:5174");
        window.location.href = `https://localhost:7093/api/Identity/account/login-google?returnUrl=${returnUrl}`;
    };
    return (
        <div style={styles.page}>
            <div style={styles.bgOrb} />
            <div style={styles.bgGrid} />

            <div style={styles.wrapper}>
                {/* Left Panel */}
                <div style={styles.leftPanel}>
                    <div style={styles.badge}>منصة إدارة المشاريع</div>
                    <h1 style={styles.headline}>
                        أدِر مشاريعك
                        <br />
                        <span style={styles.headlineAccent}>بإتقان حقيقي</span>
                    </h1>
                    <p style={styles.subtext}>
                        تتبع المهام، تواصل مع فريقك، وأنجز أكثر في وقت أقل.
                    </p>
                    <div style={styles.features}>
                        {[
                            { icon: "⚡", text: "تحديثات فورية للمهام" },
                            { icon: "🎯", text: "إدارة السبرينتات بـ Scrum" },
                            { icon: "👥", text: "تعاون الفريق في مكان واحد" },
                        ].map((f, i) => (
                            <div key={i} style={styles.featureItem}>
                                <span style={styles.featureIcon}>{f.icon}</span>
                                <span style={styles.featureText}>{f.text}</span>
                            </div>
                        ))}
                    </div>
                </div>

                {/* Right Card */}
                <div style={styles.card}>
                    <div style={styles.logoRow}>
                        <div style={styles.logoMark}>
                            <svg width="22" height="22" viewBox="0 0 28 28" fill="none">
                                <path d="M14 2L26 8V20L14 26L2 20V8L14 2Z" stroke="#6366f1" strokeWidth="2.5" fill="none" />
                                <path d="M14 8L20 11V17L14 20L8 17V11L14 8Z" fill="#6366f1" opacity="0.7" />
                            </svg>
                        </div>
                        <span style={styles.logoText}>متقن</span>
                    </div>

                    <h2 style={styles.cardTitle}>مرحباً بك</h2>
                    <p style={styles.cardSub}>سجّل دخولك للمتابعة</p>

                    <button
                        onClick={handleGoogleLogin}
                        disabled={loading}
                        style={{ ...styles.googleBtn, ...(hovering ? styles.googleBtnHover : {}) }}
                        onMouseEnter={() => setHovering(true)}
                        onMouseLeave={() => setHovering(false)}
                    >
                        {loading ? (
                            <div style={styles.spinner} />
                        ) : (
                            <svg width="20" height="20" viewBox="0 0 48 48">
                                <path fill="#FFC107" d="M43.6 20H24v8h11.3C33.6 33.1 29.3 36 24 36c-6.6 0-12-5.4-12-12s5.4-12 12-12c3 0 5.8 1.1 7.9 3l5.7-5.7C34.1 6.5 29.3 4 24 4 12.9 4 4 12.9 4 24s8.9 20 20 20c11 0 20-9 20-20 0-1.3-.1-2.7-.4-4z" />
                                <path fill="#FF3D00" d="M6.3 14.7l6.6 4.8C14.6 15.1 18.9 12 24 12c3 0 5.8 1.1 7.9 3l5.7-5.7C34.1 6.5 29.3 4 24 4 16.3 4 9.7 8.3 6.3 14.7z" />
                                <path fill="#4CAF50" d="M24 44c5.2 0 9.9-1.9 13.5-5l-6.2-5.2C29.5 35.6 26.9 36 24 36c-5.2 0-9.6-3-11.3-7.3l-6.6 5.1C9.5 39.5 16.3 44 24 44z" />
                                <path fill="#1976D2" d="M43.6 20H24v8h11.3c-.8 2.4-2.4 4.4-4.5 5.8l6.2 5.2C40.9 35.4 44 30.1 44 24c0-1.3-.1-2.7-.4-4z" />
                            </svg>
                        )}
                        <span>{loading ? "جاري التوجيه..." : "متابعة مع Google"}</span>
                    </button>

                    <div style={styles.divider}>
                        <div style={styles.dividerLine} />
                        <span style={styles.dividerText}>آمن ومشفر</span>
                        <div style={styles.dividerLine} />
                    </div>

                    <div style={styles.securityNote}>
                        <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                            <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z" />
                        </svg>
                        <span>بياناتك محمية بتشفير كامل</span>
                    </div>

                    <p style={styles.terms}>
                        بتسجيل دخولك، أنت توافق على{" "}
                        <span style={styles.link}>شروط الاستخدام</span>
                        {" "}و{" "}
                        <span style={styles.link}>سياسة الخصوصية</span>
                    </p>
                </div>
            </div>
        </div>
    )
} 
const styles = {
    page: {
        minHeight: "100vh", background: "#080810",
        display: "flex", alignItems: "center", justifyContent: "center",
        fontFamily: "'Segoe UI', Tahoma, sans-serif",
        direction: "rtl", position: "relative", overflow: "hidden", padding: "20px",
    },
    bgOrb: {
        position: "fixed", top: "-300px", right: "-300px",
        width: "800px", height: "800px", borderRadius: "50%",
        background: "radial-gradient(circle, rgba(99,102,241,0.12) 0%, transparent 65%)",
        pointerEvents: "none",
    },
    bgGrid: {
        position: "fixed", inset: 0,
        backgroundImage: "linear-gradient(rgba(99,102,241,0.04) 1px, transparent 1px), linear-gradient(90deg, rgba(99,102,241,0.04) 1px, transparent 1px)",
        backgroundSize: "60px 60px", pointerEvents: "none",
    },
    wrapper: {
        display: "flex", alignItems: "center", gap: "60px",
        maxWidth: "900px", width: "100%", position: "relative", zIndex: 1,
    },
    leftPanel: { flex: 1, display: "flex", flexDirection: "column", gap: "24px" },
    badge: {
        display: "inline-flex", alignItems: "center",
        padding: "6px 14px",
        background: "rgba(99,102,241,0.1)", border: "1px solid rgba(99,102,241,0.2)",
        borderRadius: "100px", color: "#818cf8", fontSize: "12px",
        fontWeight: "600", width: "fit-content", letterSpacing: "0.5px",
    },
    headline: {
        margin: 0, fontSize: "42px", fontWeight: "800",
        color: "#fff", lineHeight: "1.2", letterSpacing: "-0.5px",
    },
    headlineAccent: {
        background: "linear-gradient(135deg, #6366f1, #a78bfa)",
        WebkitBackgroundClip: "text", WebkitTextFillColor: "transparent",
    },
    subtext: { margin: 0, fontSize: "16px", color: "rgba(255,255,255,0.45)", lineHeight: "1.7" },
    features: { display: "flex", flexDirection: "column", gap: "14px", marginTop: "8px" },
    featureItem: { display: "flex", alignItems: "center", gap: "12px" },
    featureIcon: {
        width: "36px", height: "36px",
        background: "rgba(255,255,255,0.04)", border: "1px solid rgba(255,255,255,0.08)",
        borderRadius: "10px", display: "flex", alignItems: "center",
        justifyContent: "center", fontSize: "16px", flexShrink: 0,
    },
    featureText: { fontSize: "14px", color: "rgba(255,255,255,0.6)" },
    card: {
        width: "360px", flexShrink: 0,
        background: "rgba(255,255,255,0.03)", border: "1px solid rgba(255,255,255,0.07)",
        borderRadius: "24px", padding: "40px 36px",
        backdropFilter: "blur(20px)",
        boxShadow: "0 30px 60px rgba(0,0,0,0.5), inset 0 1px 0 rgba(255,255,255,0.06)",
    },
    logoRow: { display: "flex", alignItems: "center", gap: "10px", marginBottom: "32px" },
    logoMark: {
        width: "44px", height: "44px",
        background: "rgba(99,102,241,0.1)", border: "1px solid rgba(99,102,241,0.2)",
        borderRadius: "12px", display: "flex", alignItems: "center", justifyContent: "center",
    },
    logoText: { fontSize: "22px", fontWeight: "700", color: "#fff", letterSpacing: "1px" },
    cardTitle: { margin: "0 0 6px", fontSize: "24px", fontWeight: "700", color: "#fff" },
    cardSub: { margin: "0 0 32px", fontSize: "14px", color: "rgba(255,255,255,0.4)" },
    googleBtn: {
        width: "100%", padding: "14px",
        background: "rgba(255,255,255,0.06)", border: "1px solid rgba(255,255,255,0.12)",
        borderRadius: "14px", color: "#fff", fontSize: "15px", fontWeight: "600",
        cursor: "pointer", display: "flex", alignItems: "center",
        justifyContent: "center", gap: "12px", transition: "all 0.2s ease",
        letterSpacing: "0.3px",
    },
    googleBtnHover: {
        background: "rgba(255,255,255,0.1)", borderColor: "rgba(255,255,255,0.2)",
        transform: "translateY(-1px)", boxShadow: "0 8px 20px rgba(0,0,0,0.3)",
    },
    spinner: {
        width: "20px", height: "20px",
        border: "2px solid rgba(255,255,255,0.2)",
        borderTopColor: "#fff", borderRadius: "50%",
    },
    divider: { display: "flex", alignItems: "center", gap: "12px", margin: "24px 0" },
    dividerLine: { flex: 1, height: "1px", background: "rgba(255,255,255,0.07)" },
    dividerText: { fontSize: "11px", color: "rgba(255,255,255,0.25)", whiteSpace: "nowrap" },
    securityNote: {
        display: "flex", alignItems: "center", justifyContent: "center",
        gap: "8px", color: "rgba(255,255,255,0.3)", fontSize: "12px", marginBottom: "20px",
    },
    terms: {
        textAlign: "center", fontSize: "11px",
        color: "rgba(255,255,255,0.2)", lineHeight: "1.6", margin: 0,
    },
    link: { color: "rgba(99,102,241,0.7)", cursor: "pointer" },
};    

export default App
