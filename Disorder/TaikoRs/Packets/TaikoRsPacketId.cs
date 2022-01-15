namespace Disorder.TaikoRs.Packets;

public enum TaikoRsPacketId : ushort {
    /// <summary>
    ///     We dont know what packet this is
    /// </summary>
    Unknown = 0,

    #region Ping

    Ping = 1,
    Pong = 2,

    #endregion

    #region Login

    ClientUserLogin = 100,
    ServerLoginResponse = 101,
    ServerPermissions = 102,
    ServerUserJoined = 103,
    ClientLogOut = 104,
    ServerUserLeft = 105,

    #endregion

    #region Status Updates

    ClientStatusUpdate = 200,
    ServerUserStatusUpdate = 201,
    ClientNotifyScoreUpdate = 202,
    ServerScoreUpdate = 203,

    #endregion

    #region Chat

    ClientSendMessage = 300,
    ServerSendMessage = 301,

    #endregion

    #region Spectator

    ClientSpectate = 400,
    ServerSpectatorJoined = 401,
    ClientLeaveSpectator = 402,
    ServerSpectatorLeft = 403,
    ClientSpectatorFrames = 404,
    ServerSpectatorFrames = 405,
    ServerSpectatorPlayingRequest = 406,

    #endregion
}