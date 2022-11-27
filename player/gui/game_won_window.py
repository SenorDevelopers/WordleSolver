from constants import WINDOW_WON_DIALOG_TITLE, WINDOW_WON_DIALOG_WIDTH, WINDOW_WON_DIALOG_HEIGHT
import customtkinter

class GameWonWindow(customtkinter.CTk):
    def __init__(self, main_window):
        super().__init__()
        self.__mainWin = main_window
        self.__configure_window()
        self.__configure_frame()

    def __configure_window(self):
        self.title(WINDOW_WON_DIALOG_TITLE)
        self.geometry(f"{WINDOW_WON_DIALOG_WIDTH}x{WINDOW_WON_DIALOG_HEIGHT}")
        self.resizable(False, False)
        self.protocol("WM_DELETE_WINDOW", self.__on_closing)

        self.grid_columnconfigure(1, weight=1)
        self.grid_rowconfigure(1, weight=1)

    def __configure_frame(self): 
        self.frame_left = customtkinter.CTkFrame(master=self,width=WINDOW_WON_DIALOG_WIDTH,corner_radius=10)
        self.frame_left.grid(row=0, column=0, sticky="nswe")

        self.label_1 = customtkinter.CTkLabel(master=self.frame_left,text="You won the game!",text_font=("Roboto Medium", -16))  # font name and size in px
        self.label_1.grid(row=1, column=0, pady=10, padx=10)

        self.button_1 = customtkinter.CTkButton(master=self.frame_left,text="Try Again",command=self.reset_game)
        self.button_1.grid(row=2, column=0, pady=10, padx=20)

        self.button_2 = customtkinter.CTkButton(master=self.frame_left,text="Quit",command=self.__quit)
        self.button_2.grid(row=3, column=0, pady=10, padx=20)

    def __on_closing(self) -> None:
        self.destroy()
        exit()
    
    def reset_game(self) -> None:
        try:
            self.__mainWin.reset() # Click animation error bug
            self.destroy()
        except Exception as e:
            pass
    
    def __quit(self) -> None:
        self.destroy()
        exit()
        