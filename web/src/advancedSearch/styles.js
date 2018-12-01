const styles = theme => ({
    button: {
        marginTop: 7,
        color: 'white',
        backgroundColor: '#3f51b5',
        fontSize: '0.875rem',
        borderRadius: 4,
        textTransform: 'uppercase',
        padding: '8px 16px',
        textAlign: 'center',
        width: "100%"
    },
    searchForm: {
        flexBasis: 520,
        padding: "16px 14px",
        display: "flex",
        flexDirection: "column",
    },
    formRow: {
        display: "flex",
        justifyContent:"space-between",
        alignItems: "center",
        flexWrap: "wrap",
        margin: "12px 0px"
    },
    firstRow: {
        display: "flex",
        justifyContent:"space-between",
        alignItems: "center",
        flexWrap: "wrap",
    },
    icon: {
        fontSize: 'inherit',
        color: 'rgba(0, 0, 0, 0.54);'
    },
    iconPoi: {
        fontSize: 32,
        color: '#3FB8AF',
        marginRight: 15
    },
    link: {
        cursor: "pointer"
    },
    lessOption: {
        alignSelf: "flex-start",
    },
    sliderLabel: {
        width: 100,
        maxWidth: "100%"
    },
    pointOfInterest: {
        display: "flex",
        alignItems: "center",
        cursor: "pointer"
    },
    dropdown: {
            width: 290,
            maxWidth: "100%"
    }
});

export default styles;