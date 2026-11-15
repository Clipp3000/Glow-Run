################################################################################
## Neon Terminal Lite – free Ren'Py GUI theme 1.0.0
##
## Drop this file and the folder gui/neon_terminal_lite/ into the game/ folder of
## any Ren'Py 8 project. It restyles four screens: the main menu, the textbox
## (say screen with name tag), the choice menu and the quick menu. Every other
## screen stays the project's own.
##
## The full Neon Terminal kit styles every standard screen (game menu, save and
## load, preferences, history, help, confirm, NVL, notify, input and more):
## https://juliusschmidt.itch.io/neon-terminal-renpy-gui
## Remove this file and gui/neon_terminal_lite/ before installing the full kit.
################################################################################

init offset = 1


################################################################################
## Colours and fonts (change them here)
################################################################################

define nel_void_color = "#05060A"     ## the black behind everything
define nel_panel_color = "#0B1420"    ## textbox and choice frames
define nel_glow_color = "#CFFAFF"     ## text
define nel_dim_color = "#5B8A99"      ## secondary text, idle buttons
define nel_grid_color = "#17465A"     ## frame lines, grid, scanlines
define nel_cyan_color = "#19E6F0"     ## title and lines
define nel_magenta_color = "#FF2BD6"  ## hover and name tag

define nel_font = "gui/neon_terminal_lite/fonts/IBMPlexMono-Regular.ttf"
define nel_bold_font = "gui/neon_terminal_lite/fonts/IBMPlexMono-Medium.ttf"

## Set to False to switch the scanlines, the glitchy title and the hover pulse off.
define nel_effects = True


################################################################################
## Helpers
################################################################################

init -1 python:

    if getattr(gui, "neon_terminal_kit", None):
        raise Exception("Neon Terminal Lite and the full Neon Terminal kit are both installed. "
            "Remove neon_terminal_lite.rpy and the folder gui/neon_terminal_lite/.")

    def nel_S(n):
        """A size written in 1080p pixels, scaled to the game's resolution."""
        return int(n * config.screen_width / 1920.0 + 0.5)

    def nel_B(n):
        """A border inside the art, in 1080p pixels."""
        return max(1, int(n * (1.0 if config.screen_width >= 1600 else 1280.0 / 1920.0) + 0.5))

    def nel_art(name, color, alpha=1.0):
        res = "1080" if config.screen_width >= 1600 else "720"
        path = "gui/neon_terminal_lite/%s/%s.png" % (res, name)
        if not renpy.loadable(path):
            raise Exception("Neon Terminal Lite: the image %s is missing. Copy the folder gui/neon_terminal_lite/ "
                            "into your game folder." % path)
        return Transform(path, matrixcolor=TintMatrix(color), alpha=alpha)

    def nel_frame(name, color, border, alpha=1.0, tile=False):
        b = [nel_B(v) for v in border]
        return Frame(nel_art(name, color, alpha), b[0], b[1], b[2], b[3], tile=tile)

    def nel_panel(fill, line, big=False, fill_alpha=0.94):
        art, b = (("frame_fill", "frame_edge"), (60, 60, 60, 60)) if big else (("panel_fill", "panel_edge"), (40, 40, 40, 40))
        return Fixed(nel_frame(art[0], fill, b, fill_alpha), nel_frame(art[1], line, b))


init 3 python:
    for _f in (nel_font, nel_bold_font):
        if not renpy.loadable(_f):
            raise Exception("Neon Terminal Lite: the font %s is missing. Copy the folder gui/neon_terminal_lite/ "
                            "into your game folder." % _f)


transform nel_pulse:
    alpha 1.0
    block:
        linear 0.6 alpha (0.55 if nel_effects else 1.0)
        linear 0.6 alpha 1.0
        repeat

transform nel_title_fx:
    xoffset 0 matrixcolor IdentityMatrix()
    block:
        pause 2.8
        xoffset (-nel_S(6) if nel_effects else 0) matrixcolor (TintMatrix(nel_magenta_color) if nel_effects else IdentityMatrix())
        pause 0.05
        xoffset (nel_S(4) if nel_effects else 0)
        pause 0.04
        xoffset 0 matrixcolor IdentityMatrix()
        pause 2.4
        repeat

transform nel_band_fx:
    alpha (0.07 if nel_effects else 0.0) ypos -0.15
    linear 6.0 ypos 1.0
    repeat


################################################################################
## Main menu
################################################################################

screen main_menu():

    tag menu

    add Solid(nel_void_color)
    add nel_art("grid", nel_grid_color)
    add nel_art("vignette", "#000000")
    if nel_effects:
        add Frame(nel_art("scanlines", "#000000", 0.35), 0, 0, tile=True)
        add nel_art("band", nel_cyan_color) at nel_band_fx

    if gui.show_name:
        vbox:
            xpos nel_S(150)
            ypos nel_S(150)
            spacing nel_S(14)
            text "[config.name!t]" style "nel_title" at nel_title_fx
            add nel_art("title_line", nel_cyan_color)
            text "[config.version]" style "nel_version"

    vbox:
        style_prefix "nel_nav"
        xpos nel_S(160)
        ypos nel_S(540)
        spacing nel_S(4)

        textbutton _("Start") action Start()
        textbutton _("Load") action ShowMenu("load")
        textbutton _("Preferences") action ShowMenu("preferences")
        textbutton _("About") action ShowMenu("about")
        if renpy.variant("pc") or (renpy.variant("web") and not renpy.variant("mobile")):
            textbutton _("Help") action ShowMenu("help")
        if renpy.variant("pc"):
            textbutton _("Quit") action Quit(confirm=not main_menu)

style nel_title is default:
    font nel_bold_font
    size nel_S(120)
    color nel_cyan_color

style nel_version is default:
    font nel_font
    size nel_S(22)
    color nel_dim_color
    left_padding nel_S(8)

style nel_nav_button is default:
    xminimum nel_S(320)
    ysize nel_S(56)
    padding (nel_S(28), nel_S(2), nel_S(28), nel_S(4))
    hover_background At(nel_frame("bar_hover", nel_magenta_color, (40, 0, 40, 0)), nel_pulse)

style nel_nav_button_text is default:
    font nel_font
    size nel_S(32)
    idle_color nel_dim_color
    hover_color nel_magenta_color
    insensitive_color Color(nel_dim_color, alpha=0.45)
    yalign 0.5


################################################################################
## Say screen
################################################################################

screen say(who, what):

    window:
        id "window"

        if who is not None:

            window:
                id "namebox"
                style "nel_namebox"
                text who id "who"

        text what id "what"

    if not renpy.variant("small"):
        add SideImage() xalign 0.0 yalign 1.0

## Ren'Py's Character gives the say window the style "window", so the Lite restyles that style
## (the input screen of your project uses it too and gets the same textbox).
style window:
    xfill True
    xmargin nel_S(150)
    bottom_margin nel_S(74)
    yalign 1.0
    ysize nel_S(280)
    background nel_panel(nel_panel_color, nel_cyan_color, True, 0.92)

style nel_namebox is default:
    xpos nel_S(40)
    ypos nel_S(-34)
    padding (nel_S(30), nel_S(8), nel_S(64), nel_S(12))
    background nel_frame("tag", nel_magenta_color, (24, 12, 48, 12))

style say_label:
    font nel_bold_font
    size nel_S(30)
    color nel_void_color
    yalign 0.5

style say_dialogue:
    font nel_font
    size nel_S(32)
    color nel_glow_color
    xpos nel_S(72)
    xsize nel_S(1460)
    ypos nel_S(66)
    line_spacing nel_S(6)
    adjust_spacing False


################################################################################
## Choice screen
################################################################################

screen choice(items):
    style_prefix "nel_choice"

    vbox:
        for i in items:
            textbutton i.caption action i.action

style nel_choice_vbox is vbox:
    xalign 0.5
    ypos nel_S(430)
    yanchor 0.5
    spacing nel_S(22)

style nel_choice_button is default:
    xsize nel_S(1040)
    padding (nel_S(48), nel_S(26), nel_S(48), nel_S(26))
    background nel_panel(nel_panel_color, nel_grid_color, False, 0.92)
    hover_background At(Fixed(nel_frame("panel_fill", nel_magenta_color, (40, 40, 40, 40), 0.16), nel_frame("panel_glow", nel_magenta_color, (40, 40, 40, 40))), nel_pulse)

style nel_choice_button_text is default:
    font nel_font
    size nel_S(30)
    idle_color nel_glow_color
    hover_color nel_magenta_color
    insensitive_color Color(nel_dim_color, alpha=0.45)
    xalign 0.5
    textalign 0.5
    layout "subtitle"


################################################################################
## Quick menu
################################################################################

screen quick_menu():

    zorder 100

    if quick_menu:

        hbox:
            style_prefix "nel_quick"
            xalign 0.5
            yalign 1.0
            yoffset -nel_S(16)
            spacing nel_S(30)

            textbutton _("Back") action Rollback()
            textbutton _("History") action ShowMenu('history')
            textbutton _("Skip") action Skip() alternate Skip(fast=True, confirm=True)
            textbutton _("Auto") action Preference("auto-forward", "toggle")
            textbutton _("Save") action ShowMenu('save')
            textbutton _("Q.Save") action QuickSave()
            textbutton _("Q.Load") action QuickLoad()
            textbutton _("Prefs") action ShowMenu('preferences')

style nel_quick_button is default:
    padding (nel_S(8), nel_S(4), nel_S(8), nel_S(10))
    hover_background At(nel_frame("underline", nel_magenta_color, (12, 0, 12, 0)), nel_pulse)

style nel_quick_button_text is default:
    font nel_font
    size nel_S(22)
    idle_color nel_dim_color
    hover_color nel_magenta_color
    selected_color nel_cyan_color
    insensitive_color Color(nel_dim_color, alpha=0.45)
